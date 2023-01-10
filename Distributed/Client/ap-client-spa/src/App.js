import { Route, Routes } from "react-router-dom";
import Layout from "./components/Layout/Layout";
import IdentityPage from "./pages/IdentityPage";

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/identity" element={<IdentityPage />} />
      </Routes>
    </Layout>
  );
}

export default App;
