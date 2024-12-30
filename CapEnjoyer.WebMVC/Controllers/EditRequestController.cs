namespace Cap.Enjoyer.WebMVC.Controllers;

using CapEnjoyer.BL.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Models;

public class EditRequestController(IEditRequestService editRequestService) : Controller
{
    [HttpGet]
    public IActionResult Index(string message)
    {
        var viewModel = new EditRequestMessageViewModel { Message = message };

        return View(viewModel);
    }


    [HttpGet]
    public async Task<IActionResult> Bottle()
    {
        var info = await editRequestService.GetBottleEditRequestInfo();

        if (info.EditRequestCount == 0 || info.FirstEditRequest is null)
        {
            return RedirectToAction(nameof(Index), new { message = "No bottle requests are here" });
        }
        if (info.CurrentEntity is null)
        {
            return RedirectToAction(nameof(Index), new { message = "Missing original bottle entity of this request" });
        }

        var viewModel = info.Adapt<EditRequestBottleViewModel>();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Bottle(EditRequestConfirmationModel returnModel)
    {
        if (ModelState.IsValid)
        {
            await editRequestService.ConfirmBottleEdit(returnModel.CurrentEntityId, returnModel.EditRequestId, returnModel.IsEditConfirmed);
        }

        return RedirectToAction(nameof(Bottle));
    }

    [HttpGet]
    public async Task<IActionResult> Cap()
    {
        var info = await editRequestService.GetCapEditRequestInfo();

        if (info.EditRequestCount == 0 || info.FirstEditRequest is null)
        {
            return RedirectToAction(nameof(Index), new { message = "No cap requests are here" });
        }
        if (info.CurrentEntity is null)
        {
            return RedirectToAction(nameof(Index), new { message = "Missing original cap entity of this request" });
        }

        var viewModel = info.Adapt<EditRequestCapViewModel>();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Cap(EditRequestConfirmationModel returnModel)
    {
        if (ModelState.IsValid)
        {
            await editRequestService.ConfirmCapEdit(returnModel.CurrentEntityId, returnModel.EditRequestId, returnModel.IsEditConfirmed);
        }

        return RedirectToAction(nameof(Cap));
    }
    [HttpGet]
    public async Task<IActionResult> Producer()
    {
        var info = await editRequestService.GetProducerEditRequestInfo();

        if (info.EditRequestCount == 0 || info.FirstEditRequest is null)
        {
            return RedirectToAction(nameof(Index), new { message = "No producer requests are here" });
        }
        if (info.CurrentEntity is null)
        {
            return RedirectToAction(nameof(Index), new { message = "Missing original producer entity of this request" });
        }

        var viewModel = info.Adapt<EditRequestProducerViewModel>();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Producer(EditRequestConfirmationModel returnModel)
    {
        if (ModelState.IsValid)
        {
            await editRequestService.ConfirmProducerEdit(returnModel.CurrentEntityId, returnModel.EditRequestId, returnModel.IsEditConfirmed);
        }

        return RedirectToAction(nameof(Producer));
    }
}
