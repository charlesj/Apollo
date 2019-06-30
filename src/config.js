export const getRootConfig = () => ({
  db: {
    pgUri: process.env.MONGODB_URI,
    dbName: process.env.DB_NAME
  },
  server: {
    port: process.env.PORT || 8042
  }
})
