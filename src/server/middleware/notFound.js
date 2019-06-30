import { notFound } from '../http'

// TODO include resource in notFound payload
export function handleNotFound(req, res) {
  notFound(res)
}
