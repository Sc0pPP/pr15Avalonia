using System.Collections.Generic;
using pr15Avalonia.Models;

namespace pr15Avalonia;

public static class CurrentBuild
{
    
    public static Cpu SelectedCpu { get; set; }
    public static Gpu SelectedGpu { get; set; }
    public static Motherboard SelectedMotherboard { get; set; }
    public static Ram SelectedRam { get; set; }
    public static Case SelectedCase { get; set; }
    public static Powersupply SelectedPowerSupply { get; set; }
    public static Processorcooler SelectedProcessorCooler { get; set; }
    public static List<Storagedevice> SelectedStorageDevices { get; set; } = new List<Storagedevice>();
    
    public static Basepart CpuBasePart { get; set; }
    public static Basepart GpuBasePart { get; set; }
    public static Basepart MotherboardBasePart { get; set; }
    public static Basepart RamBasePart { get; set; }
    public static Basepart CaseBasePart { get; set; }
    public static Basepart PowerSupplyBasePart { get; set; }
    public static Basepart ProcessorCoolerBasePart { get; set; }
    
    
    public static decimal GetTotalPrice()
    {
        decimal total = 0;
        
        if (CpuBasePart != null) total += CpuBasePart.Price;
        if (GpuBasePart != null) total += GpuBasePart.Price;
        if (MotherboardBasePart != null) total += MotherboardBasePart.Price;
        if (RamBasePart != null) total += RamBasePart.Price;
        if (CaseBasePart != null) total += CaseBasePart.Price;
        if (PowerSupplyBasePart != null) total += PowerSupplyBasePart.Price;
        if (ProcessorCoolerBasePart != null) total += ProcessorCoolerBasePart.Price;
        
        foreach (var storage in SelectedStorageDevices)
        {
            var storagePart = storage.IdNavigation;
            if (storagePart != null) total += storagePart.Price;
        }
        
        return total;
    }
    
    public static void Reset()
    {
        SelectedCpu = null;
        SelectedGpu = null;
        SelectedMotherboard = null;
        SelectedRam = null;
        SelectedCase = null;
        SelectedPowerSupply = null;
        SelectedProcessorCooler = null;
        SelectedStorageDevices.Clear();
        
        CpuBasePart = null;
        GpuBasePart = null;
        MotherboardBasePart = null;
        RamBasePart = null;
        CaseBasePart = null;
        PowerSupplyBasePart = null;
        ProcessorCoolerBasePart = null;
    }
}