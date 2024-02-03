using System;
using System.ComponentModel.DataAnnotations;

[Flags]
public enum IntoxicationEffect
{
    [Display(Name = "Impares Driving")]
    ImparesDriving = 1,
    [Display(Name = "Never Tired")]
    InfiniteStamina = 2,
    [Display(Name = "Invincibility")]
    Invincibility = 4,
    [Display(Name = "Impares Walking")]
    ImparesWalking = 8,
    [Display(Name = "Super Speed")]
    FastSpeed = 16,
    [Display(Name = "Muscle Relaxant")]
    RelaxesMuscles = 32,
    Unused4 = 64,
}