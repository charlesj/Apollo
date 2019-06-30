import React from 'react'
import PropTypes from 'prop-types'
import { connect, } from 'react-redux'
import { Field, reduxForm, } from 'redux-form'
import Flexbox from 'flexbox-react'
import {
  CancelButton,
  SaveButton,
  Container,
  RemoveButton,
} from '../_controls'
import './accountForm.css'

function AccountForm(props) {
  const { handleSubmit, onCancel, onDelete, } = props
  return (
    <Container className="accountFormContainer" width={400}>
      <form onSubmit={handleSubmit}>
        <Flexbox flexDirection='column'>
          <Field
            name="name"
            component="input"
            type="text"
            placeholder="Name"
          />
          <Field
            name="type"
            component="input"
            type="text"
            placeholder="Account Type"
          />
          <Field
            name="description"
            component="textarea"
            type="text"
            placeholder="Description"
          />
          <Flexbox justifyContent="flex-end" flexGrow={1}>
            <CancelButton type="button" onClick={onCancel} />
            {onDelete && <RemoveButton onClick={onDelete} />}
            <SaveButton type="submit" primary />
          </Flexbox>
        </Flexbox>
      </form>
    </Container>
  )
}

AccountForm.propTypes = {
  account: PropTypes.object.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onDelete: PropTypes.func,
  handleSubmit: PropTypes.func.isRequired,
}

AccountForm = reduxForm({ // eslint-disable-line no-func-assign
  form: 'accountForm',
  enableReinitialize: true,
})(AccountForm)

function mapStateToProps(state, props) {
  const { account, } = props

  return {
    initialValues: account,
  }
}

export default connect(mapStateToProps)(AccountForm)