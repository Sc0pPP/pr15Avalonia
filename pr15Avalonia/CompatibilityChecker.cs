using System.Collections.Generic;
using System.Linq;
using pr15Avalonia.Models;

namespace pr15Avalonia;
public static class CompatibilityChecker
{
    // Проверка совместимости процессора и материнской платы (сокет)
    public static bool IsCpuCompatibleWithMotherboard(Cpu cpu, Motherboard motherboard)
    {
        if (cpu == null || motherboard == null) return true;
        return cpu.Socketid == motherboard.Socketid;
    }
    
    // Проверка совместимости оперативной памяти и материнской платы
    public static bool IsRamCompatibleWithMotherboard(Ram ram, Motherboard motherboard)
    {
        if (ram == null || motherboard == null) return true;
        
        // Проверка типа памяти (DDR4/DDR5)
        if (ram.Memorytypeid != motherboard.Memorytypeid)
            return false;
            
        // Проверка количества модулей памяти
        if (ram.Count > motherboard.Memoryslots)
            return false;
            
        return true;
    }
    
    // Проверка совместимости материнской платы и корпуса (форм-фактор)
    public static bool IsMotherboardCompatibleWithCase(Motherboard motherboard, Case caseItem, PcDbContext context)
    {
        if (motherboard == null || caseItem == null) return true;
        
        // Проверяем, поддерживает ли корпус форм-фактор материнской платы
        var compatible = context.Boardformfactorcases
            .Any(bfc => bfc.Caseid == caseItem.Id && bfc.Formfactorid == motherboard.Formfactorid);
            
        return compatible;
    }
    
    // Проверка совместимости видеокарты и корпуса (количество слотов расширения)
    public static bool IsGpuCompatibleWithCase(Gpu gpu, Case caseItem)
    {
        if (gpu == null || caseItem == null) return true;
        
        // Обычно видеокарта занимает 2-3 слота
        // Можно добавить это поле в таблицу GPU, пока предположим 2 слота
        int gpuSlots = 2;
        
        return caseItem.Expansionslots >= gpuSlots;
    }
    
    // Проверка совместимости видеокарты и материнской платы (PCI слоты)
    public static bool IsGpuCompatibleWithMotherboard(Gpu gpu, Motherboard motherboard)
    {
        if (gpu == null || motherboard == null) return true;
        
        return motherboard.Pcislots >= 1;
    }
    
    // Проверка мощности блока питания
    public static bool IsPowerSupplySufficient(Powersupply psu, Cpu cpu, Gpu gpu)
    {
        if (psu == null) return true;
        
        int requiredPower = 0;
        
        // Добавляем TDP процессора
        if (cpu != null)
            requiredPower += cpu.Thermalpower;
            
        // Добавляем рекомендуемую мощность видеокарты
        if (gpu != null && gpu.Recommendpower.HasValue)
            requiredPower += gpu.Recommendpower.Value;
        else if (gpu != null)
            requiredPower += 300; // Запас по умолчанию
            
        // Добавляем запас на остальные компоненты (материнка, память, накопители)
        requiredPower += 150;
        
        // Рекомендуется запас 20%
        return psu.Power >= requiredPower * 1.2m;
    }
    
    // Проверка совместимости кулера и процессора (сокет)
    public static bool IsCoolerCompatibleWithCpu(Processorcooler cooler, Cpu cpu, PcDbContext context)
    {
        if (cooler == null || cpu == null) return true;
    
        var compatible = context.Socketprocessorcoolers
            .Any(spc => spc.Processorcoolerid == cooler.Id && spc.Socketid == cpu.Socketid);
        
        return compatible;
    }
    
    // Проверка количества накопителей
    public static bool IsStorageCompatible(List<Storagedevice> storageDevices, Motherboard motherboard)
    {
        if (motherboard == null || storageDevices == null || !storageDevices.Any()) 
            return true;
        
        int sataDevices = storageDevices.Count(sd => sd.Storagedeviceinterfaceid == 1); // SATA
        
        return sataDevices <= motherboard.Sataports;
    }
    
    // Комплексная проверка всей сборки
    public static List<string> ValidateCurrentBuild(PcDbContext context)
    {
        var errors = new List<string>();
        
        // Проверка CPU и материнской платы
        if (CurrentBuild.SelectedCpu != null && CurrentBuild.SelectedMotherboard != null)
        {
            if (!IsCpuCompatibleWithMotherboard(CurrentBuild.SelectedCpu, CurrentBuild.SelectedMotherboard))
            {
                errors.Add($"Процессор (сокет {CurrentBuild.SelectedCpu.Socket?.Name}) несовместим с материнской платой (сокет {CurrentBuild.SelectedMotherboard.Socket?.Name})");
            }
        }
        
        // Проверка RAM и материнской платы
        if (CurrentBuild.SelectedRam != null && CurrentBuild.SelectedMotherboard != null)
        {
            if (!IsRamCompatibleWithMotherboard(CurrentBuild.SelectedRam, CurrentBuild.SelectedMotherboard))
            {
                errors.Add("Оперативная память несовместима с материнской платой (неправильный тип или слишком много модулей)");
            }
        }
        
        // Проверка материнской платы и корпуса
        if (CurrentBuild.SelectedMotherboard != null && CurrentBuild.SelectedCase != null)
        {
            if (!IsMotherboardCompatibleWithCase(CurrentBuild.SelectedMotherboard, CurrentBuild.SelectedCase, context))
            {
                errors.Add("Материнская плата не подходит по форм-фактору к корпусу");
            }
        }
        
        // Проверка GPU и корпуса
        if (CurrentBuild.SelectedGpu != null && CurrentBuild.SelectedCase != null)
        {
            if (!IsGpuCompatibleWithCase(CurrentBuild.SelectedGpu, CurrentBuild.SelectedCase))
            {
                errors.Add("В корпусе недостаточно слотов расширения для видеокарты");
            }
        }
        
        // Проверка мощности БП
        if (CurrentBuild.SelectedPowerSupply != null)
        {
            if (!IsPowerSupplySufficient(CurrentBuild.SelectedPowerSupply, CurrentBuild.SelectedCpu, CurrentBuild.SelectedGpu))
            {
                errors.Add("Мощность блока питания недостаточна для выбранной конфигурации");
            }
        }
        
        // Проверка кулера и CPU
        if (CurrentBuild.SelectedProcessorCooler != null && CurrentBuild.SelectedCpu != null)
        {
            if (!IsCoolerCompatibleWithCpu(CurrentBuild.SelectedProcessorCooler, CurrentBuild.SelectedCpu, context))
            {
                errors.Add("Кулер несовместим с сокетом процессора");
            }
        }
        
        // Проверка накопителей
        if (CurrentBuild.SelectedStorageDevices.Any() && CurrentBuild.SelectedMotherboard != null)
        {
            if (!IsStorageCompatible(CurrentBuild.SelectedStorageDevices, CurrentBuild.SelectedMotherboard))
            {
                errors.Add("Слишком много SATA накопителей для материнской платы");
            }
        }
        
        return errors;
    }
}