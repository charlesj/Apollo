import React from 'react';
import moment from 'moment';
import apolloServer from '../../services/apollo-server';

class Finance extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      assetPrices:[]
    };

    this.loadAssetPrices = this.loadAssetPrices.bind(this);
  }

  componentDidMount(){
    this.loadAssetPrices();
  }

  loadAssetPrices(){
    var assets = ["btc", "eth", "ltc"];
    assets.forEach(a => {
      apolloServer.invoke('getAssetPrice', {symbol: a})
        .then(price => {
          var currentPrices = this.state.assetPrices;
          currentPrices.push(price);
          currentPrices.sort((a,b) => {
            return a.symbol > b.symbol;
          });
          this.setState({assetPrices: currentPrices});
      });
    });
  }

  render() {
    return (<div className="summaryContainer">
        { this.state.assetPrices.map((ap) => {
            var validAt = moment(ap.valid_at);
      return (<div key={ap.id} className="summary">
                <div className="summaryAmount">${ap.price}</div>
                <div className="summaryLabel">{ap.symbol.toUpperCase()} ({validAt.calendar()})</div>
            </div>);
        })}
    </div>)
  }
}

module.exports = Finance;
