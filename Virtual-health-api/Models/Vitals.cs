using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Virtual_health_api.Models;

[FirestoreData]
public class Vitals
{
    [FirestoreProperty("activityLevel")]
    public string ActivityLevel { get; set; } = string.Empty;

    [FirestoreProperty("bloodPressure")]
    public string BloodPressure { get; set; } = string.Empty;

    [FirestoreProperty("caloriesBurned")]
    public double? CaloriesBurned { get; set; }

    [FirestoreProperty("date")]
    public string Date { get; set; } = string.Empty;

    [FirestoreProperty("heartRate")]
    public int? HeartRate { get; set; }

    [FirestoreProperty("mood")]
    public string Mood { get; set; } = string.Empty;

    [FirestoreProperty("note")]
    public string Note { get; set; } = string.Empty;

    [FirestoreProperty("oxygenLevel")]
    public int? OxygenLevel { get; set; }

    [FirestoreProperty("sleepHours")]
    public double? SleepHours { get; set; }

    [FirestoreProperty("steps")]
    public int? Steps { get; set; }

    [FirestoreProperty("stressLevel")]
    public string StressLevel { get; set; } = string.Empty;

    [FirestoreProperty("tags")]
    public List<string>? Tags { get; set; }

    [FirestoreProperty("timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [FirestoreProperty("user_id")]
    public string User_Id { get; set; } = string.Empty;
}
