using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VendingMachine.API.Dto.Drinks;
using VendingMachine.Core.Exceptions;
using VendingMachine.Core.Models;
using VendingMachine.Core.Models.Exceptions;
using VendingMachine.Core.Services;

namespace VendingMachine.API.Controllers.Http
{
    [ApiController]
    [Authorize, Route("[controller]")]
    public class DrinksController(DrinksService drinksService) : Controller
    {
        private readonly DrinksService drinksService = drinksService;

        [HttpPost("buy")]
        public async Task<ActionResult<BuyDrinkResponse>> Buy([FromBody] BuyDrinkRequest req)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var drinks = req.DrinksInOrder.Select(d => new Drink(d.Id, d.Price, d.Title, null!, d.CountInCart));

                var change = await drinksService.Buy(
                     req.UserId,
                     req.VendingMachineId,
                     req.AmountDeposited,
                     drinks);

                return Ok(new BuyDrinkResponse(change));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "Admin"), HttpPost]
        public async Task<ActionResult<Guid>> Add([FromForm] AddDrinkRequest req)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var idDrink = await drinksService.Add(
                    req.PriceDrink,
                    req.TitleDrink,
                    req.ImageDrink.OpenReadStream(),
                    req.CountDrink,
                    req.VendingMachineId);

                return Ok(idDrink);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "Admin"), HttpPatch]
        public async Task<ActionResult<Guid>> Update([FromForm] UpdateDrinkRequest req)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var idDrink = await drinksService.Update(
                    req.DrinkId,
                    req.TitleDrink!,
                    req.PriceDrink!.Value,
                    req.ImageDrink?.OpenReadStream(),
                    req.CountDrink!.Value);

                return Ok(idDrink);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [Authorize(Roles = "Admin"), HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> Delete([FromRoute] Guid id)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var idDrink = await drinksService.Delete(id);

                return Ok(idDrink);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetDrinkResponse?>> GetById([FromRoute] Guid id)
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var drink = await drinksService.GetById(id);

                return Ok(drink);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDrinkResponse>?>> GetAll()
        {
            HttpContext.Response.ContentType = "application/json";

            try
            {
                var drinks = await drinksService.GetAll();

                return Ok(drinks);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ExceptionResponse((int)HttpStatusCode.NotFound, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ExceptionResponse((int)HttpStatusCode.BadRequest, ex.Message));
            }
        }
    }
}
