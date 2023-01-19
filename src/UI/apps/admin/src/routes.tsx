import React from 'react';
import { Route, Routes, Navigate } from 'react-router-dom';
import Projects from './views/projects';
import Templates from './views/templates';
import Jobs from './views/jobs';
import TicketsTimers from './views/tickets-timers';
import TicketIndex from './views/ticket-index';
import CreateTicket from './views/create-ticket';

export const AppRoutes: React.FC = () => {
  return (
    <Routes>
      <Route path="projects" element={<Projects />} />
      <Route path="templates" element={<Templates />} />
      <Route path="jobs" element={<Jobs />} />
      <Route path="ticket-timers" element={<TicketsTimers />} />
      <Route path="ticket-index" element={<TicketIndex />} />
      <Route path="create-ticket" element={<CreateTicket />} />
      <Route
        path="*"
        element={<Navigate replace to="/projects"></Navigate>}
      ></Route>
    </Routes>
  );
};
