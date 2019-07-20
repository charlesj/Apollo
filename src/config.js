export const getRootConfig = () => ({
  db: {
    pgHOST: process.env.POSTGRES_HOST || 'postgres',
    dbName: process.env.DB_NAME || 'apollo'
  },
  server: {
    port: process.env.PORT || 8042
  }
})
