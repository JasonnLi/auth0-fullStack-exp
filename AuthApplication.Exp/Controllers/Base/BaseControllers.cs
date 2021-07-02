using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Application.Controllers
{
    // the two empty marker abstract controllers are use as flags for which “branch” should the controller belong to.

    public abstract class ApiControllerBase : ControllerBase { }

    // public abstract class PrivateControllerBase : ControllerBase { }
}