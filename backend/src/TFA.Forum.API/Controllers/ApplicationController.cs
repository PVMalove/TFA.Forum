﻿using Microsoft.AspNetCore.Mvc;

namespace TFA.Forum.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApplicationController : ControllerBase {}