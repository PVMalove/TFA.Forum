using System.Reflection;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Extensions
{
    /// <summary>
    /// Валидирует только TRequest с возвращаемым типом Result&lt;T, ErrorList&gt; или UnitResult&lt;ErrorList&gt;.
    /// </summary>
    /// <typeparam name="TResponse">Должен быть <see cref="Result{T, ErrorList}"/> или <see cref="UnitResult{ErrorList}"/>.</typeparam>
    public class ResultValidationPipelineBehavior<TRequest, TResponse>(IValidator<TRequest> validator)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
                return await next();

            // Создаем ErrorList на основе результатов валидации
            var errorList = CreateErrorList(validationResult);
            
            // Обрабатываем разные типы ответов
            if (IsResultType())
                return CreateResultResponse(errorList);

            if (IsUnitResultType())
                return CreateUnitResultResponse(errorList);

            return await next();
        }
        private static ErrorList CreateErrorList(ValidationResult validationResult) =>
            new(validationResult.Errors.Select(e => Error.Deserialize(e.ErrorMessage)));

        private bool IsResultType() =>
            typeof(TResponse).IsGenericType && 
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<,>) && 
            typeof(TResponse).GetGenericArguments()[1] == typeof(ErrorList);

        private bool IsUnitResultType() =>
            typeof(TResponse).IsGenericType && 
            typeof(TResponse).GetGenericTypeDefinition() == typeof(UnitResult<>) && 
            typeof(TResponse).GetGenericArguments()[0] == typeof(ErrorList);

        private TResponse CreateResultResponse(ErrorList errorList)
        {
            var successType = typeof(TResponse).GetGenericArguments()[0];
            var resultType = typeof(Result<,>).MakeGenericType(successType, typeof(ErrorList));
            
            var constructor = resultType.GetConstructor(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance, // Ищем нестандартные (не публичные) экземплярные конструкторы
                    null, // Не указываем модификатор типа
                    new[] { typeof(bool), typeof(ErrorList), successType }, // Параметры конструктора
                    null)!
                .Invoke(new object[] { true, errorList, null }); // Вызываем конструктор

            return (TResponse)constructor;
        }

        private TResponse CreateUnitResultResponse(ErrorList errorList)
        {
            var resultType = typeof(UnitResult<>).MakeGenericType(typeof(ErrorList));
            
            var constructor = resultType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, // Ищем нестандартные (не публичные) экземплярные конструкторы
                    null, // Не указываем модификатор типа
                    new[] { typeof(bool), typeof(ErrorList).MakeByRefType() }, // Параметры конструктора
                    null)!;
            
            var result = constructor
                .Invoke(new object[] { true, errorList }); // Вызываем конструктор

            return (TResponse)result;
        }
    }
}