import React from 'react';
import { Settings, Menu } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import { useUI } from '../../contexts/UIContext';
import { NotificationBell } from './NotificationBell';

export const Header: React.FC = () => {
  const { toggleMobileSidebar } = useUI();
  const navigate = useNavigate();

  return (
    <header className="sticky top-0 z-20 bg-white border-b border-coffee-100 shadow-sm">
      <div className="flex items-center justify-between px-6 py-4">
        <div className="flex-1 flex items-center">
          <button
            onClick={toggleMobileSidebar}
            className="md:hidden p-2 rounded-lg hover:bg-coffee-50 transition-colors mr-2"
          >
            <Menu className="h-5 w-5 text-coffee-600" />
          </button>
        </div>

        <div className="flex-1 flex items-center justify-end gap-2">
          <NotificationBell />
          <button
            onClick={() => navigate('/settings')}
            className="p-2 rounded-lg hover:bg-coffee-50 transition-colors"
            title="Configuración"
          >
            <Settings className="h-5 w-5 text-coffee-600" />
          </button>
        </div>
      </div>
    </header>
  );
};
