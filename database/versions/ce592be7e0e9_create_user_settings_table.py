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

    op.execute(
        "insert into user_settings(name, value, created_at, updated_at) " +
        "values ('password_hash','XsOa/YTsT8nwXpAE5+wQIx7WPkDjiY3DNeZS+k" +
        "NcWKMYsitrXKS3sgs9IpCjAKVUyqQtv0969ZsAMuQaTSDrpffHfhW1ag/eX3Wao" +
        "ykSdADgtvnwrwLx7EvmGdpuOFnSEph5AmsrYzip426BqDHOBKCI+VnzmSRZDv20" +
        "2bfgSna4RNuc9bCC1DiNOCKPPTlIshM6bB1UrFdV31y0VAbidD/WRS3a6+T0ibZ" +
        "zwOu7OZDatCMGArIvHDgNvwqcyIkz',current_timestamp,current_timestamp)")


def downgrade():
    op.drop_table('user_settings')
