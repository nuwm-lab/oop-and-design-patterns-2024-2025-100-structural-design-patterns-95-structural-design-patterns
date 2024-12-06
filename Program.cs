using System;
using System.Collections.Generic;
using System.Linq;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 
    public class AnalogSignal 
    {
        public double Amplitude { get; set; } // Максимальна амплітуда (В)
        public double Frequency { get; set; } // Частота (Гц)
        public double Phase { get; set; } // Фаза (радіани)

        public AnalogSignal(double amplitude, double frequency, double phase)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            Phase = phase;
        }
    }
    public class DigitalSignal
    {
        private double SamplingFrequency { get; set; } // Частота дискретизації (Гц)
        private double Discharge { get; set; } // Розрядність АЦП
        private List<double> TimeInterval { get; set; } // Часовий інтервал
        private List<double> Samples { get; set; } // Дискретні значення сигналу

        public DigitalSignal(double samplingFrequency, double discharge, List<double> timeInterval,List<double> samples)
        {
            this.SamplingFrequency = samplingFrequency;
            this.Discharge = discharge;
            this.TimeInterval = timeInterval;
            this.Samples = samples;
        }
        public DigitalSignal()
        {
            
        }
        public virtual double GetSamplingFrequency()
        {
            return this.SamplingFrequency;
        }
        public virtual double GetDischarge()
        {
            return this.Discharge;
        }
        public virtual List<double> GetTimeInterval()
        {
            return this.TimeInterval;
        }
        public virtual List<double> GetSamples()
        {
            return this.Samples;
        }


    }

    public class DigitalAnalogAdapter : DigitalSignal
    {
        public AnalogSignal Signal { get; set; }
        public DigitalAnalogAdapter(AnalogSignal signal):base()
        {
            Signal = signal;
        }
        //За формулами з інтернету переводимо значення аналогового сигналу в цифровий
        public override double GetSamplingFrequency()
        {
            return 2 * Signal.Frequency;
        }
        public override double GetDischarge()
        {
            return Math.Log2(Signal.Amplitude / 0.01);
        }
        public override List<double> GetTimeInterval()
        {
            var TimeInterval = new List<double>();
            for (double t = 0; t <= 1 / Signal.Frequency; t += 1 / GetSamplingFrequency())
            {
                TimeInterval.Add(t);
            }
            return TimeInterval;
        }
        public override List<double> GetSamples()
        {
            var Samples = new List<double>();
            for (double t = 0; t <= 1 / Signal.Frequency; t += 1 / GetSamplingFrequency())
            {
                Samples.Add(Signal.Amplitude * Math.Sin(2 * Math.PI * Signal.Frequency * t + Signal.Phase));
            }
            return Samples;
        }
    }
    

    class Program
    {
        public static void ShowDigitalSignal(DigitalSignal signal) // Метод приймає екземпляр класу DigitalSignal, тому потрібно використати адаптер
        {
            Console.WriteLine("*** Digital signal ***");
            Console.WriteLine($"Sampling of frequency : {signal.GetSamplingFrequency()}");
            Console.WriteLine($"Discharge : {signal.GetDischarge()}");
            Console.Write($"Time interval : ");
            foreach (var item in signal.GetTimeInterval())
            {
                if (signal.GetTimeInterval().Last() == item)
                    Console.Write($" {item}.");
                else
                    Console.Write($" {item} |");
            }
            Console.WriteLine();
            Console.Write($"Samples : ");
            foreach (var item in signal.GetSamples())
            {
                if (signal.GetSamples().Last() == item)
                    Console.Write($" {item}.");
                else
                    Console.Write($" {item} |");
            }
        }
        static void Main(string[] args)
        {
            AnalogSignal analogSignal = new AnalogSignal(5, 70, 130);

            DigitalAnalogAdapter adapter = new DigitalAnalogAdapter(analogSignal);

            ShowDigitalSignal(adapter);
        }
    }
}
