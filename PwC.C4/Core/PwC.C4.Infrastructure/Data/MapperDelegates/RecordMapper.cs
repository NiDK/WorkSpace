using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Infrastructure.Data.MapperDelegates
{
    /// <summary>
    /// For mapping individual records from a single-resultset procedure.
    /// </summary>
    /// <param name="record"></param>
    public delegate void RecordMapper(IRecord record);

    /// <summary>
    /// For mapping individual records from a single resultset procedure to an object instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record"></param>
    /// <param name="objectInstance"></param>
    public delegate void RecordMapper<T>(IRecord record, T objectInstance);

    public delegate void RecordDynamicMapper<T>(IRecord record, dynamic objectInstance, T obj);

    /// <summary>
    /// For mapping individual records from a multiple resultset procedure to an object instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="record"></param>
    /// <param name="objectInstance"></param>
    /// <param name="recordSetIndex"></param>
    public delegate void MrsRecordMapper<T>(IRecord record, int recordSetIndex, T objectInstance);

    /// <summary>
    /// For mapping entire results from single or multi-resultset procedures.
    /// </summary>
    /// <param name="record"></param>
    public delegate void ResultMapper(IRecordSet record);

    /// <summary>
    /// For injecting parameters into a command.
    /// </summary>
    /// <param name="parameters"></param>
    public delegate void ParameterMapper(IParameterSet parameters);

    /// <summary>
    /// For injecting parameters from an object instance into a command.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameters"></param>
    /// <param name="objectInstance"></param>
    public delegate void ParameterMapper<T>(IParameterSet parameters, T objectInstance);

    /// <summary>
    /// For populating output parameters
    /// </summary>
    /// <param name="outputParameters"></param>
    public delegate void OutputParameterMapper(IParameterSet outputParameters);

    /// <summary>
    /// For populating output parameters from an object instance - added for unit testing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="outputParameters"></param>
    /// <param name="objectInstance"></param>
    public delegate void OutputParameterMapper<T>(IParameterSet outputParameters, T objectInstance);
}
