"""Create journal table

Revision ID: b738513b07d5
Revises:
Create Date: 2017-01-11 04:48:30.913102

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'b738513b07d5'
down_revision = None
branch_labels = None
depends_on = None


def upgrade():
    op.create_table(
        'journal',
        sa.Column('id', sa.Integer, primary_key=True),
        sa.Column('note', sa.Unicode(5000), nullable=False)
    )


def downgrade():
    op.drop_table('journal')
