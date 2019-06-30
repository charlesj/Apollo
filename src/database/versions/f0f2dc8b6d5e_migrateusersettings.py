"""MigrateUserSettings

Revision ID: f0f2dc8b6d5e
Revises: cdb2997e1c4b
Create Date: 2018-02-03 19:21:56.771064

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'f0f2dc8b6d5e'
down_revision = 'cdb2997e1c4b'
branch_labels = None
depends_on = None


def upgrade():
    op.execute(
        "update user_settings set value='S1duK/y+6eW40UCHEfl1enGqRL6LL2CDTZ" +
        "Cf648e9IuYkCxvPEV4YP9LG0gM6enl5j1/GGmnE+EYG/ZEMkzk9xU4Bg3GFirLnaYe" +
        "FZmVBmf5xz727klwG14G+biWyXtTu7DFL7q1FGECzUuOeWdS4gBQgHIhR5RMZMd6M5" +
        "+j8Ud1aD4zHkgUTCcab4lOr+dk3fYteftrc+pArVDa6KocYyVubms8LeJ9OiyRtFmU" +
        "s83OsqNMiryCDEJDa0ligU/2' where name='password_hash'")


def downgrade():
    op.execute(
        "update user_settings set " +
        "value='{\"wrapper\":\"S1duK/y+6eW40UCHEfl1enG" +
        "qRL6LL2CDTZCf648e9IuYkCxvPEV4YP9LG0gM6enl5j1/GGmnE+EYG/ZEMkzk9x" +
        "U4Bg3GFirLnaYeFZmVBmf5xz727klwG14G+biWyXtTu7DFL7q1FGECzUuOeWdS4" +
        "gBQgHIhR5RMZMd6M5+j8Ud1aD4zHkgUTCcab4lOr+dk3fYteftrc+pArVDa6Koc" +
        "YyVubms8LeJ9OiyRtFmUs83OsqNMiryCDEJDa0ligU/2\"}' " +
        "where name='password_hash'")
