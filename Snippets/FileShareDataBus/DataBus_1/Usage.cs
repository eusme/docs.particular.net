﻿using NServiceBus.ClaimCheck;

class Usage
{
    Usage(NServiceBus.EndpointConfiguration endpointConfiguration, string databusPath)
    {
        #region FileShareDataBus

        var dataBus = endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
        dataBus.BasePath(databusPath);

        #endregion
    }
}
