using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class ReflectionDayModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DayofYear { get; set; }
        public List<ChapterModel> Chapters { get; set; }
        public string Footer { get; set; }
        public string References { get; set; }
        public List<AnnotationModel> Annotations { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFavorite { get; set; }
        public string Image { get; set; }
        public string OpeningSentence { get; set; }
        public int Index { get; set; }
        public List<WorkoutModel> Workouts { get; set; }
        public byte[] Resume { get; set; }
        public string Activity { get; set; }
        public List<string> Penance { get; set; }
        public ExerciciosModel WorkoutInfo { get; set; }
        public TypeWorkout TypeWorkout { get; set; }
    }

    public class ReflectionDayWrapper
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime DayofYear { get; set; }
        public List<ChapterModel> Chapters { get; set; }
        public string Footer { get; set; }
        public string References { get; set; }
        public List<AnnotationModel> Annotations { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsFavorite { get; set; }
        public string Image { get; set; }
        public string OpeningSentence { get; set; }
        public int Index { get; set; }
        public byte[] Resume { get; set; }
        public string Activity { get; set; }
        public List<WorkoutModel> Workouts { get; set; }
        public List<string> Penance { get; set; }
        public ExerciciosModel WorkoutInfo { get; set; }
        public TypeWorkout TypeWorkout { get; set; }
    }

    public enum TypeWorkout
    {
        Default,
        WorkoutA,
        WorkoutB,
    }

    public class WorkoutModel
    {
        public string Name { get; set; }
        public string Prescrition { get; set; }
        public List<string> Observations { get; set; }
        public AlternativeWorkoutModel Alternative { get; set; }

    }

    public class Workout
    {
        [BsonId]
        public string Name { get; set; }
        public string Days { get; set; }
        public string URL { get; set; }
        public List<WorkoutModel> WorkoutList { get; set; }
    }

    public class ExerciciosModel
    {
        [BsonId]
        public string Name { get; set; }
        public string Days { get; set; }
        public string URL { get; set; }
        public List<WorkoutModel> WorkoutList { get; set; }
    }

    public class AlternativeWorkoutModel
    {
        public string Name { get; set; }
        public string Prescrition { get; set; }

        public List<string> Observations { get; set; }
    }
    public class ChapterModel
    {
        public string Description { get; set; }
        public string Reference { get; set; }
        public List<string> SubChapters { get; set; }
    }

    public class AnnotationModel
    {
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PenanceModel
    {
        public int Week { get; set; }
        public string Description { get; set; }
    }


}
