import 'bootstrap/dist/css/bootstrap.min.css';
import MyNavbar from './components/MyNavbar';
import Footer from './components/Footer';
import Content from './components/Content';

function App() {
  return (
    <div className="App">
      <MyNavbar />
      <Content />
      <Footer />
    </div>
  );
}

export default App;
