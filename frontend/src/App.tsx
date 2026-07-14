import { lazy, Suspense } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

// Contexts
import { AuthProvider, UIProvider, SettingsProvider, NotificationsProvider, PuntoVentaProvider } from './contexts';
import { ToastProvider } from './components/ui';
import { PageLoader } from './components/ui/PageLoader';
 
// Pages — lazy loaded
const DashboardPage        = lazy(() => import('./pages/DashboardPage'));
const ProductsPage         = lazy(() => import('./pages/inventory/ProductsPage'));
const CategoriesPage       = lazy(() => import('./pages/inventory/CategoriesPage'));
const AdjustmentsPage      = lazy(() => import('./pages/inventory/AdjustmentsPage'));
const KardexPage           = lazy(() => import('./pages/inventory/KardexPage'));
const ElaboradosPage       = lazy(() => import('./pages/inventory/ElaboradosPage'));
const CombosPage           = lazy(() => import('./pages/inventory/CombosPage'));
const VariacionesPage      = lazy(() => import('./pages/inventory/VariacionesPage'));
const SalesListPage        = lazy(() => import('./pages/sales/SalesListPage').then(m => ({ default: m.SalesListPage })));
const POSPage              = lazy(() => import('./pages/sales/POSPage').then(m => ({ default: m.POSPage })));
const CustomersPage        = lazy(() => import('./pages/sales/CustomersPage').then(m => ({ default: m.CustomersPage })));

const FidelizacionPage            = lazy(() => import('./pages/fidelizacion/FidelizacionPage').then(m => ({ default: m.FidelizacionPage })));
const PromocionesPermanentesPage  = lazy(() => import('./pages/fidelizacion/PromocionesPermanentesPage').then(m => ({ default: m.PromocionesPermanentesPage })));
const PromocionesTemporadaPage    = lazy(() => import('./pages/fidelizacion/PromocionesTemporadaPage').then(m => ({ default: m.PromocionesTemporadaPage })));
const HitosPage                   = lazy(() => import('./pages/fidelizacion/HitosPage').then(m => ({ default: m.HitosPage })));
const SorteosPage                 = lazy(() => import('./pages/fidelizacion/SorteosPage').then(m => ({ default: m.SorteosPage })));
const ReferidosPage               = lazy(() => import('./pages/fidelizacion/ReferidosPage').then(m => ({ default: m.ReferidosPage })));
const ConfiguracionPuntosPage     = lazy(() => import('./pages/fidelizacion/ConfiguracionPuntosPage').then(m => ({ default: m.ConfiguracionPuntosPage })));
const ProductosCanjeablesPage     = lazy(() => import('./pages/fidelizacion/ProductosCanjeablesPage').then(m => ({ default: m.ProductosCanjeablesPage })));
const NotificacionesPage          = lazy(() => import('./pages/fidelizacion/NotificacionesPage').then(m => ({ default: m.NotificacionesPage })));
const PurchaseOrdersPage   = lazy(() => import('./pages/purchases/PurchaseOrdersPage').then(m => ({ default: m.PurchaseOrdersPage })));
const SuppliersPage        = lazy(() => import('./pages/purchases/SuppliersPage').then(m => ({ default: m.SuppliersPage })));
const CashRegisterPage     = lazy(() => import('./pages/cash/CashRegisterPage').then(m => ({ default: m.CashRegisterPage })));
const SalesReportPage      = lazy(() => import('./pages/reports/SalesReportPage'));
const InventoryReportPage  = lazy(() => import('./pages/reports/InventoryReportPage'));
const PurchasesReportPage  = lazy(() => import('./pages/reports/PurchasesReportPage'));
const CashReportPage       = lazy(() => import('./pages/reports/CashReportPage'));
const DailyCashReportPage       = lazy(() => import('./pages/reports/DailyCashReportPage'));
const MonthlyProductsReportPage = lazy(() => import('./pages/reports/MonthlyProductsReportPage'));
const SettingsQRPage       = lazy(() => import('./pages/settings/SettingsQRPage').then(m => ({ default: m.SettingsQRPage })));
const InsumosPage          = lazy(() => import('./pages/recipes/InsumosPage'));
const RecetasPage          = lazy(() => import('./pages/recipes/RecetasPage'));

function App() {
  return (
    <AuthProvider>
      <PuntoVentaProvider>
        <SettingsProvider>
          <UIProvider>
            <ToastProvider>
              <NotificationsProvider>
                <BrowserRouter>
                  <Suspense fallback={<PageLoader />}>
                    <Routes>
                      {/* Dashboard — solo admin */}
                      <Route path="/" element={<DashboardPage />} />

                      {/* Inventory — admin + cajero */}
                      <Route path="/inventory/products"    element={<ProductsPage />} />
                      <Route path="/inventory/categories"  element={<CategoriesPage />} />
                      <Route path="/inventory/adjustments" element={<AdjustmentsPage />} />
                      <Route path="/inventory/kardex"      element={<KardexPage />} />
                      <Route path="/inventory/elaborados"  element={<ElaboradosPage />} />
                      <Route path="/inventory/combos"      element={<CombosPage />} />
                      <Route path="/inventory/variations"  element={<VariacionesPage />} />

                      {/* Sales */}
                      <Route path="/sales/pos"       element={<POSPage />} />
                      <Route path="/sales"           element={<SalesListPage />} />
                      <Route path="/sales/customers" element={<CustomersPage />} />

                      {/* Fidelización — admin + cajero */}
                      <Route path="/fidelizacion"                         element={<FidelizacionPage />} />
                      <Route path="/fidelizacion/config"                  element={<ConfiguracionPuntosPage />} />
                      <Route path="/fidelizacion/productos"               element={<ProductosCanjeablesPage />} />
                      <Route path="/fidelizacion/notificaciones"          element={<NotificacionesPage />} />
                      <Route path="/fidelizacion/promociones-permanentes" element={<PromocionesPermanentesPage />} />
                      <Route path="/fidelizacion/promociones-temporada"   element={<PromocionesTemporadaPage />} />
                      <Route path="/fidelizacion/hitos"                   element={<HitosPage />} />
                      <Route path="/fidelizacion/sorteos"                 element={<SorteosPage />} />
                      <Route path="/fidelizacion/referidos"               element={<ReferidosPage />} />

                      {/* Purchases — solo admin */}
                      <Route path="/purchases/orders"    element={<PurchaseOrdersPage />} />
                      <Route path="/purchases/suppliers" element={<SuppliersPage />} />

                      {/* Cash — admin + cajero */}
                      <Route path="/cash/register" element={<CashRegisterPage />} />

                      {/* Reports — solo admin */}
                      <Route path="/reports/sales"     element={<SalesReportPage />} />
                      <Route path="/reports/inventory" element={<InventoryReportPage />} />
                      <Route path="/reports/purchases" element={<PurchasesReportPage />} />
                      <Route path="/reports/cash"      element={<CashReportPage />} />
                      <Route path="/reports/daily"     element={<DailyCashReportPage />} />
                      <Route path="/reports/monthly"   element={<MonthlyProductsReportPage />} />

                      {/* Recipes — admin + cajero */}
                      <Route path="/recipes/insumos" element={<InsumosPage />} />
                      <Route path="/recipes/recetas" element={<RecetasPage />} />

                      {/* Settings */}
                      <Route path="/settings" element={<SettingsQRPage />} />

                      {/* Fallback */}
                      <Route path="*" element={<Navigate to="/" replace />} />
                    </Routes>
                  </Suspense>
                </BrowserRouter>
              </NotificationsProvider>
            </ToastProvider>
          </UIProvider>
        </SettingsProvider>
      </PuntoVentaProvider>
    </AuthProvider>
  );
}

export default App;
