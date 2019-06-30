export function removePoweredBy(req, res, next){
  res.removeHeader('X-Powered-By')
  next()
}
