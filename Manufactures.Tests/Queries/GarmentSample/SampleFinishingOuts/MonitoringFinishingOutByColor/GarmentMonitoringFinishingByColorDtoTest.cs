using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries.GarmentSampleFinishingByColorMonitoring;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleFinishingOuts.MonitoringFinishingOutByColor
{
	public class GarmentMonitoringFinishingByColorDtoTest
	{
		[Fact]
		public void Should_Success_Intantiate()
		{
			GarmentSampleFinishingByColorMonitoringDto garmentMonitoring = new GarmentSampleFinishingByColorMonitoringDto();
			GarmentSampleFinishingByColorMonitoringDto dto = new GarmentSampleFinishingByColorMonitoringDto(garmentMonitoring);

			Assert.NotNull(dto);

		}
	}
}
