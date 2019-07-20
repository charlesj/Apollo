exports.shorthands = undefined;

exports.up = async (pgm) => {
  await pgm.createTable(
    'settings',
    {
      id: "id",
      key: { type: 'varchar(256)', notNull: true },
      value: { type: 'text', notNull: true },
      createdAt: { type: 'timestamp', notNull: true, default: pgm.func('current_timestamp') },
      updatedAt: { type: 'timestamp', notNull: true, default: pgm.func('current_timestamp') },
    }
  )
};

exports.down = (pgm) => {
  pgm.dropTable('settings')
};
