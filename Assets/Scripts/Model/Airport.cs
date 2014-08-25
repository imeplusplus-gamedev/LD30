﻿using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class Airport : MonoBehaviour
{
    public List<Plane> PlaneList;
    public Dictionary<Airport, int> AirportPassengerCountDictionary;

    public int passengers
    {
        get { return AirportPassengerCountDictionary.Any() ? AirportPassengerCountDictionary.Values.Sum() : 0; }
    }

	public int capacity = 10;

    private GameObject pin, pinSelected;
    private List<Airport> avaiableAirports;

    private Circle circle;

    private bool Active
    {
        get { return gameObject.activeSelf; }
    }

    float timer = 1.0f;

    void Start()
    {
        circle = transform.FindChild("Circle").GetComponent<Circle>();

        capacity = Constants.instance.airportCapacity;

        pin = transform.FindChild("Pin").gameObject;
        pinSelected = transform.FindChild("Pin Select").gameObject;

        AirportPassengerCountDictionary = new Dictionary<Airport, int>();
        var airports = GameObject.FindObjectOfType<AirportList>();
        if (airports != null)
        {
            avaiableAirports = airports.available;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            AddPassengers();    
            timer = 1.0f;
        }
        


    }

    void AddPassengers()
    {
        if (!Active) return;
        var newPassengers = new Random().Next(5,10);
        
        if (passengers + newPassengers >= capacity)
        {
            //TODO: O que fazer se estourar a capacidade
            return;
        }
        var airportIndex = new Random().Next(0, avaiableAirports.Count);
        
        
        if (airportIndex == avaiableAirports.IndexOf(this))
        {
            airportIndex = (airportIndex + 1) % avaiableAirports.Count;
        }
        var airportTo = avaiableAirports[airportIndex];
        //Debug.Log("Airport: " + gameObject.name + " destination: " + airportTo.gameObject.name + " passengers: " + passengers + " newPassengers: " + newPassengers + " capacity: " + capacity);
        if (AirportPassengerCountDictionary.ContainsKey(airportTo))
        {
            AirportPassengerCountDictionary[airportTo] += newPassengers;
        }
        else
        {
            AirportPassengerCountDictionary.Add(airportTo, newPassengers);
        }

    }


    public void ChangeAnimation(bool isSelected)
    {
        if (isSelected)
        {
            pin.SetActive(false);
            pinSelected.SetActive(true);
        }
        else
        {
            pin.SetActive(true);
            pinSelected.SetActive(false);
        }
    }

    public bool HaveClicked(Vector3 clickPosition)
    {
        var circleCenter = transform.position;
        var vectorDiff = circleCenter - clickPosition;
        vectorDiff.z = 0;

        var width = Constants.instance.circleSize.x * circle.size / 2.0f;
        var radius = Constants.instance.airportMinCircle;
        radius = width < radius ? radius : width;
        return vectorDiff.sqrMagnitude < radius * radius;
    }

}
