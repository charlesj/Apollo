export function byName(state, name) {
  return state.metrics.metrics.filter(m => m.name === name);
}

export function chartData(state, name, startDate){
  const metrics = state.metrics.metrics.filter(m => m.created_at < startDate)
}
