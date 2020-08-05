using WeatherForecastAPI.Services;
using Xunit;
using System;
using WeatherForecastAPI.Entities;
using System.Collections.Generic;
using System.Linq;

namespace WeatherApiUnitTesting
{
    public class ServicesTests
    {
        private readonly WeatherServices _sut;

        public ServicesTests()
        {
            _sut = new WeatherServices();
        }

        [Fact]
        public void TestGetForecastsByDateAndCityId()
        {
            //Arrange-arrange what i use
            DateTime fromDate = new DateTime(2020, 07, 25);
            DateTime toDate = new DateTime(2020, 07, 27);
            long cityId=1;
            List<Forecasts> forecasts = new List<Forecasts>
            {
                new Forecasts
                {
                    CitiesId=1,
                    CreatedDate=new DateTime(2020, 07, 25),
                    ForecastTime=new DateTime(2020,07,25, 1, 1, 1),
                    Provider="BBC",
                    Temperature=25,
                    Id=1
                },
                new Forecasts
                {
                    CitiesId=1,
                    CreatedDate= new DateTime(2020,7,25),
                    ForecastTime=new DateTime(2020,8,25, 1, 1, 1),
                    Provider="BBC",
                    Temperature=27,
                    Id=2
                },
                new Forecasts
                {
                    CitiesId=1,
                    CreatedDate= new DateTime(2020,7,25),
                    ForecastTime=new DateTime(2020,7,28, 1, 1, 1),
                    Provider="METEO",
                    Temperature=27,
                    Id=3
                }
            };

            //Act - the call of the method
            List<Forecasts> data = _sut.GetForecasts(fromDate, toDate, cityId, forecasts);
            //Assert - assert values from act

            List<Forecasts> expectedresult = new List<Forecasts>
            {
                new Forecasts
                {
                    CitiesId=1,
                    CreatedDate=new DateTime(2020, 07, 25),
                    ForecastTime=new DateTime(2020,07,25, 1, 1, 1),
                    Provider="BBC",
                    Temperature=25,
                    Id=1
                }
            };

            bool isequal = expectedresult.First().Id == data.First().Id
                && expectedresult.First().CitiesId == data.First().CitiesId
                && expectedresult.First().CreatedDate == data.First().CreatedDate
                && expectedresult.First().Provider == data.First().Provider
                && expectedresult.First().Temperature == data.First().Temperature
                && expectedresult.First().ForecastTime == data.First().ForecastTime;

            //Assert.Equal(expectedresult, data);
            Assert.True(isequal);
        }
    }
}