using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Controllers;

namespace API.Controllers
{
  public class ActivitiesController : BaseApiController
  {
    [HttpGet]
    public async Task<IActionResult> GetActivities()
    {
      return HandleResult(await Mediator.Send(new List.Query()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(Guid id)
    {

      return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity)
    {
      return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
    }

    // 引数　id, activity
    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity)
    {
      // Mediatorに渡す前に  activity の Id を put した id に変える
      activity.Id = id;
      return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
      return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }
  }
}