import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'

import { ChecklistItemTypes, } from '../../redux/constants'
import { Field, FieldArray, reduxForm, } from 'redux-form'
import {
  CancelButton,
  SaveButton,
  AddButton,
  TextButton,
  FormSelect,
  Container,
  RemoveButton,
} from '../_controls'

function ChecklistItemInput({ fields, }) {
  return (
    <div>
      {fields.map((item, index) => {
        return (
          <div key={index} className="formItemContainer">
            <div>
              <TextButton
                className="removeButton"
                onClick={() => fields.remove(index)}
              >
                remove
              </TextButton>
              <Field
                name={`${item}.name`}
                component="input"
                type="text"
                placeholder="Item name"
              />
            </div>
            <div>
              <Field
                name={`${item}.type`}
                component={FormSelect}
                options={ChecklistItemTypes.options()}
              />
            </div>
            <div>
              <Field
                name={`${item}.description`}
                component="textarea"
                type="text"
                placeholder="Item description"
              />
            </div>
          </div>
        )
      })}
      <AddButton noun="Item" onClick={() => fields.push()} />
    </div>
  )
}

ChecklistItemInput.propTypes = {
  fields: PropTypes.array.isRequired,
}

class ChecklistForm extends Component {
  render() {
    const { handleSubmit, onCancel, onDelete, } = this.props
    return (
      <Container className="checklistFormContainer" width={400}>
        <form onSubmit={handleSubmit}>
          <div>
            <Field
              name="name"
              component="input"
              type="text"
              placeholder="Title"
            />
          </div>
          <Field
            name="description"
            component="textarea"
            type="text"
            placeholder="Description"
          />
          <div>
            <FieldArray name="items" component={ChecklistItemInput} />
          </div>
          <CancelButton type="button" onClick={onCancel} />
          <RemoveButton onClick={onDelete} />
          <SaveButton type="submit" primary />
        </form>
      </Container>
    )
  }
}

ChecklistForm.propTypes = {
  checklist: PropTypes.object.isRequired,
  handleSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
}

ChecklistForm = reduxForm({ // eslint-disable-line no-class-assign
  form: 'checklistForm',
  enableReinitialize: true,
})(ChecklistForm)

function mapStateToProps(state, props) {
  const { checklist, } = props

  return {
    initialValues: checklist,
  }
}

export default connect(mapStateToProps)(ChecklistForm)
