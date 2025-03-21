import React from 'react';
import logo from './logo.svg';
import Typography from '@mui/material/Typography';
import './App.css';
import { MovieList } from './components/MovieList/MovieList';

export const devApi = "https://localhost:32783/";

function App() {

  return (
    <div className="App">
      {/* <Container maxWidth="sm" color="primary">
      
      </Container> */}
      <header className="App-header">
      <Typography variant="h6" gutterBottom>
        Movie Cost Compare
      </Typography>
      <MovieList>

      </MovieList>
      </header>
    </div>
  );
}

export default App;
