"""Create user settings table

Revision ID: ce592be7e0e9
Revises: e5c73fd17a7f
Create Date: 2017-05-03 01:16:44.644888

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'ce592be7e0e9'
down_revision = 'e5c73fd17a7f'
branch_labels = None
depends_on = None

def upgrade():
    op.create_table(
        'user_settings',
        sa.Column('id', sa.Integer, primary_key=True, autoincrement=True),
        sa.Column('name', sa.String(100), nullable=False),
        sa.Column('value', sa.Text(), nullable=False),
        sa.Column('created_at', sa.DateTime(), nullable=False),
        sa.Column('updated_at', sa.DateTime(), nullable=False)
    )

    op.create_unique_constraint("uq_setting_name", "user_settings", ["name"])

    op.execute(
        "insert into user_settings(name, value, created_at, updated_at) " +
        "values ('password_hash','{\"wrapper\":\"S1duK/y+6eW40UCHEfl1enG" +
        "qRL6LL2CDTZCf648e9IuYkCxvPEV4YP9LG0gM6enl5j1/GGmnE+EYG/ZEMkzk9x" +
        "U4Bg3GFirLnaYeFZmVBmf5xz727klwG14G+biWyXtTu7DFL7q1FGECzUuOeWdS4" +
        "gBQgHIhR5RMZMd6M5+j8Ud1aD4zHkgUTCcab4lOr+dk3fYteftrc+pArVDa6Koc" +
        "YyVubms8LeJ9OiyRtFmUs83OsqNMiryCDEJDa0ligU/2\"}'" +
        ",current_timestamp,current_timestamp)")


def downgrade():
    op.drop_constraint("uq_setting_name", "user_settings", "unique")
    op.drop_table('user_settings')
