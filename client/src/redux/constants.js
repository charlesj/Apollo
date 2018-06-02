export const ChecklistItemTypes = {
  mandatory: 'Mandatory',
  recommended: 'Recommended',
  optional: 'Optional',
}

ChecklistItemTypes.all = () => {
  return Object.keys(ChecklistItemTypes).filter(key => {
    return key !== 'all' && key !== 'options'
  })
}

ChecklistItemTypes.options = () => {
  return ChecklistItemTypes.all().map(t => {
    return { value: t, label: ChecklistItemTypes[t], }
  })
}
