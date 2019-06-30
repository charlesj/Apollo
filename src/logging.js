import winston, { format } from 'winston'
import { errorForLog } from './errors/errorForLog'

let configured = false

function write(level, message, rest) {
  const cleaned = errorForLog(rest)
  if (configured) winston.log(level, message, cleaned)
}


export function configureBasic() {
  const { combine, simple } = format
  winston.configure({
    transports: [
      new winston.transports.Console({
        level: 'debug',
        colorize: true,
        format: combine(
          simple(),
        ),
      }),
    ],
  })
  configured = true
}


export const logger = {
  info: (msg, ...rest) => write('info', msg, rest),
  error: (msg, ...rest) => write('error', msg, rest),
  warn: (msg, ...rest) => write('warn', msg, rest),
  debug: (msg, ...rest) => write('debug', msg, rest),
  success: (msg, ...rest) => write('success', msg, rest),
}
