import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navigation from "./components/Navigation/Navigation";
import Home from "./components/Home/Home";
import Footer from "./components/Footer/Footer";
import Register from "./components/Auth/Register/Register";
import Login from "./components/Auth/Login/Login";
import ErrorBoundary from "./components/Error-Boundary/Error-Boundary";
import AuthProvider from "./components/AuthProvider/AuthProvider";
import AuthenticatedUser from "./components/Route-Guards/AuthenticatedUser";
import AdminUser from "./components/Route-Guards/AdminUser";
import ReceptionistUser from "./components/Route-Guards/ReceptionistUser";
import NonAuthenticatedUser from "./components/Route-Guards/NonAuthenticatedUser";
import AdminDashboard from "./components/Dashboards/AdminDashBoard/AdminDashboard";
import ReceptionistDashBoard from "./components/Dashboards/ReceptionistDashBoard/ReceptionistDashBoard";
import NotFound404 from "./components/StatusCodePages/404/404";
import Contact from "./components/Contact/Contact";
import CreateHotel from "./components/Admin/Hotels/CreateHotel/CreateHotel";
import HotelsList from "./components/Admin/Hotels/HotelsList/HotelsList";
import HotelDetails from "./components/Hotels/HotelDetails/HotelDetails";
import EditHotel from "./components/Admin/Hotels/EditHotel/EditHotel";
import TooManyRequests429 from "./components/StatusCodePages/429/429";
import ManageRooms from "./components/Admin/Rooms/ManageRooms/ManageRooms";
import CreateRoom from "./components/Admin/Rooms/CreateRoom/CreateRoom";

function App() {
  return (
    <ErrorBoundary>
      <AuthProvider>
        <div className="app">
          <BrowserRouter>
            <Navigation />
            <main className="main-content">
              <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/contact" element={<Contact />} />
                <Route path="/hotels/:id" element={<HotelDetails />} />
                <Route path="/404" element={<NotFound404 />} />
                <Route path="/429" element={<TooManyRequests429 />} />
                <Route path="*" element={<NotFound404 />} />
                <Route element={<NonAuthenticatedUser />}>
                  <Route path="/register" element={<Register />} />
                  <Route path="/login" element={<Login />} />
                </Route>
                <Route element={<AuthenticatedUser />}>
                  {/* You have to put here all the routes that require authentication */}

                  <Route element={<AdminUser />}>
                    {/* You have to put here all the routes that require admin role */}
                    <Route
                      path="/admin-dashboard"
                      element={<AdminDashboard />}
                    />
                    <Route
                      path="/admin/hotels/create-hotel"
                      element={<CreateHotel />}
                    />
                    <Route
                      path="/admin/hotels/edit-hotel/:id"
                      element={<EditHotel />}
                    />
                    <Route path="/admin/hotels" element={<HotelsList />} />
                    <Route
                      path="/admin/hotels/:id/rooms"
                      element={<ManageRooms />}
                    />
                    <Route
                      path="/admin/hotels/:id/rooms/add-room"
                      element={<CreateRoom />}
                    />
                  </Route>
                  <Route element={<ReceptionistUser />}>
                    {/* You have to put here all the routes that require receptionist role */}
                    <Route
                      path="/receptionist-dashboard"
                      element={<ReceptionistDashBoard />}
                    />
                  </Route>
                </Route>
              </Routes>
            </main>
            <Footer />
          </BrowserRouter>
        </div>
      </AuthProvider>
    </ErrorBoundary>
  );
}

export default App;
