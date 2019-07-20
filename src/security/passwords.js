import bcrypt from 'bcryptjs'

const rounds = 10

export const secure = (password) => {
  console.log('securinttext')
  return bcrypt.hashSync(password, rounds)
}

export const compare = (attempt, encryptedPassword) => {
  return bcrypt.compareSync(attempt, encryptedPassword)
}
