﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using waw.Models;
using waw.ViewModels;

namespace waw.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {

        private readonly CommandContext _context;
        private readonly IMapper _mapper;

        public CommandsController(CommandContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }


        [HttpGet]
        public ActionResult<IEnumerable<CommandDTO>> GetCommandItems()
        {


            //return _context.CommandItems;
            return Ok(_mapper.Map<IEnumerable<CommandDTO>>(_context.CommandItems));






        }

        private ActionResult<IEnumerable<Command>> View(List<CommandDTO> dtos)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public ActionResult<CommandDTO> GetCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);

            if (commandItem == null)
            {
                return NotFound();
            }
            return _mapper.Map<CommandDTO>(commandItem);
        }
        //[HttpGet("{id}")]
        //public Command Get(int id) => _context.CommandItems.Find(id);


        [HttpPost]
        public ActionResult<CommandDTOCreate> PostCommandItem(CommandDTOCreate command_model)
        {
            var command = _mapper.Map<Command>(command_model);
            _context.CommandItems.Add(command);

            _context.SaveChanges();

            //return CreatedAtAction("GetCommandItem", new Command { Id = command.Id }, command);
            return Ok(command);
        }

        [HttpPut("{id}")]
        public ActionResult PutCommandItem(int id, CommandDTOUpdate commandDto)
        {

            var command_model = _context.CommandItems.Find(id);

            if(command_model == null)
            {
                return NotFound();
            }


            _mapper.Map(commandDto, command_model);
            _context.Entry(command_model).State = EntityState.Modified;

            _context.SaveChanges();

            return NoContent();



        }

        [HttpDelete("{id}")]
        public ActionResult<Command> DeleteCommandItem(int id)
        {
            var commandItem = _context.CommandItems.Find(id);
            if(commandItem == null)
            {
                return NotFound();

            }
            _context.CommandItems.Remove(commandItem);
            _context.SaveChanges();

            return commandItem;
        }


        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandDTOUpdate> patchDoc)
        {
            var commandItem = _context.CommandItems.Find(id);
            if (commandItem == null)
            {
                return NotFound();

            }

            var commandToPatch = _mapper.Map<CommandDTOUpdate>(commandItem);

            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandItem);
            //_context.Entry(commandItem).State = EntityState.Modified;
            _context.Update(commandItem);
            _context.SaveChanges();

            return Ok(commandItem);


        }
    }
}

