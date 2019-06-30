import serializeError from 'serialize-error'

export function errorForLog(obj) {
  if(obj instanceof Array){
    return obj.reduce((acc, val, index) => {
      acc[index] = serializeError(val)
      return acc
    }, {})
  }
  return serializeError(obj)
}
