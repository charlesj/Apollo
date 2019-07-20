import { secure } from '../security/passwords'
const main = async () => {
  console.log(secure('was'))
}

main().then(() => {
  console.log('done')
  process.exit(0)
})
