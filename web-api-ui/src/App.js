import logo from './logo.svg';
import './App.css';

import {Home} from './Home';
import {Department} from './Department';
import {Employee} from './Employee';
import {Navigation, Naviggation} from './Navigation'

import {BrowserRouter, Route, Switch} from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
    <div className="container">
      <h3 className="m-3 d-flex justify-content-center">
        React JS Tutorial
      </h3>
      <Navigation/>
      <Switch>
        <Route path='/' component={Home} exact />
        <Route path='/department' component={Department} exact />
        <Route path='/employee' component={Employee} exact />
      </Switch>
    </div>
    </BrowserRouter>
  );
}

export default App;
