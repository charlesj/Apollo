export function clacks(req, res, next) {
  res.set('X-Clacks-Overhead', 'GNU Terry Pratchet')
  next()
}
