using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class DayArgs
{
    public List<Reservations> reservationList;
    public DateTime date;
}
