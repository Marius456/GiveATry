using Serilog;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Repositories.Interfaces;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public ExerciseService(IExerciseRepository exerciseRepository, IUserRepository userRepository, AppDbContext context)
        {
            _exerciseRepository = exerciseRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<ExerciseResponse> Create(ExerciseDTO newExercise)
        {
            var exercise = ExerciseMapper.Map(newExercise);

            if (exercise.UserId == null)
            {
                string errorMessage = "Exercise user ID not found.";
                Log.Error(errorMessage);
                return new ExerciseResponse(errorMessage);
            }
            if (exercise.Text == null)
            {
                string errorMessage1 = "Exercise description not found.";
                Log.Error(errorMessage1);
                return new ExerciseResponse(errorMessage1);
            }

            try
            {
                await _exerciseRepository.Create(exercise);
                await _context.SaveChangesAsync();
                return new ExerciseResponse(ExerciseMapper.Map(exercise));
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new ExerciseResponse(errorMessage);
            }
        }


        public async Task<IEnumerable<ExerciseDTO>> GetAll()
        {
            return (await _exerciseRepository.GetAll()).Select(ExerciseMapper.Map).ToList();
        }

        public async Task<ExerciseResponse> GetById(Guid id)
        {
            var exercise = await _exerciseRepository.GetById(id);

            if (exercise == null)
            {
                string errorMessage = "Exercise not found.";

                Log.Error(errorMessage);

                return new ExerciseResponse(errorMessage);
            }

            return new ExerciseResponse(ExerciseMapper.Map(exercise));
        }

        public async Task<IEnumerable<ExerciseDTO>> GetByUser(Guid id)
        {
            return (await _exerciseRepository.GetByUser(id)).Select(ExerciseMapper.Map).ToList();
        }

        public async Task<ExerciseResponse> Update(Guid id, ExerciseDTO updatedExercise, string userMail, bool isAdmin)
        {
            var exercise = await _exerciseRepository.GetById(id);
            if (exercise == null)
            {
                throw new KeyNotFoundException();
            }

            var user = _userRepository.FindByEmail(userMail);
            if (exercise.UserId != user.Result.Id && !isAdmin)
            {
                string errorMessage = "You have no permition to edit this exercise.";
                Log.Error(errorMessage);
                return new ExerciseResponse(errorMessage, false);
            }

            exercise.Id = updatedExercise.Id;
            exercise.UserId = updatedExercise.UserId;
            exercise.Title = updatedExercise.Title;
            exercise.Text = updatedExercise.Text;

            if (exercise.UserId == null)
            {
                string errorMessage = "Exercise user ID not found.";
                Log.Error(errorMessage);
                return new ExerciseResponse(errorMessage, true);
            }
            if (exercise.Title == null)
            {
                string errorMessage1 = "Exercise title not found.";
                Log.Error(errorMessage1);
                return new ExerciseResponse(errorMessage1, true);
            }
            if (exercise.Text == null)
            {
                string errorMessage2 = "Exercise description not found.";
                Log.Error(errorMessage2);
                return new ExerciseResponse(errorMessage2, true);
            }

            try
            {
                _exerciseRepository.Update(exercise);
                await _context.SaveChangesAsync();
                return new ExerciseResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new ExerciseResponse(errorMessage, true);
            }
        }

        public async Task<ExerciseResponse> Delete(Guid id, string userMail, bool isAdmin)
        {
            var item = await _exerciseRepository.GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            var user = _userRepository.FindByEmail(userMail);
            if (item.UserId != user.Result.Id && !isAdmin)
            {
                string errorMessage = "You have no permition to edit this exercise.";
                Log.Error(errorMessage);
                return new ExerciseResponse(errorMessage, false);
            }

            try
            {
                _exerciseRepository.Delete(item);
                await _context.SaveChangesAsync();
                return new ExerciseResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new ExerciseResponse(errorMessage, true);
            }
        }

    }
}
