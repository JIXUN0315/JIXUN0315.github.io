using AdminManagement.BaseModels;
using AdminManagement.Models.ViewModels;
using AdminManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hatsukoi.Repository.EntityModel;
using Line.Messaging;
using isRock.LineBot;
using Line.Messaging.Webhooks;
using System.Collections.Generic;
using isRock.LineBot.Extensions;
using AdminManagement.Models.Dtos;

namespace AdminManagement.WebApi
{
    public class LineController : BaseApiController
    {
        private readonly LineService _lineService;
        private readonly LineBotService _lineBotService;

        public LineController(LineService lineService, LineBotService lineBotService)
        {
            _lineService = lineService;
            _lineBotService = lineBotService;
        }

        [HttpGet]
        public IActionResult GetLineMessages()
        {
            var messageData = _lineService.GetLineMessage();
            return Ok(new BaseApiResponse(messageData));
        }

        [HttpPost]
        public IActionResult Create(LineCreateViewModel data)
        {
            var result = _lineService.Create(data);
            return Ok(new BaseApiResponse() { IsSuccess = result });
        }

        [HttpPost]
        public IActionResult Boardcast(LineCreateViewModel data)
        {
            _lineBotService.BoardcastMessage(data);
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(LineViewModel data)
        {
            var result = _lineService.Delete(data.Id);
            return Ok(new BaseApiResponse() { IsSuccess = result });
        }

        [HttpPost]
        public IActionResult Edit(LineEditViewModel data)
        {
            var result = _lineService.Update(data);
            return Ok(new BaseApiResponse() { IsSuccess = result });
        }


    }
}
