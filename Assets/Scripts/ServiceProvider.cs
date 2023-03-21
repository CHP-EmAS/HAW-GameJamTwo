using InventoryController;
using UnityEngine;
using MetronomeController;


public class ServiceProvider
{
    private static ServiceProvider _instance;
    public static ServiceProvider Instance => _instance ?? (_instance = new ServiceProvider());

    public Metronome Metronome { get; private set; }
    public Inventory Inventory { get; private set; }

    private ServiceProvider()
    {
        Metronome = new Metronome();
        Inventory = new Inventory();
    }
}