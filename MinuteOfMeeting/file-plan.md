# File-Level Change Plan
## UI Reimplementation - Minutes of Meeting Management System

### Overview
Complete replacement of all view-related files with Bulma CSS framework implementation. This plan maintains all server-side functionality while rebuilding the entire presentation layer.

### Files to be Modified

#### 1. Core Infrastructure Files

| File | Action | Purpose |
|------|--------|---------|
| `Views/_ViewImports.cshtml` | Modify | Add Bulma tag helpers and remove Bootstrap references |
| `Views/_ViewStart.cshtml` | Keep unchanged | Maintain default layout specification |
| `Views/Shared/_Layout.cshtml` | Complete rewrite | New Bulma-based master layout |
| `Views/Shared/_ValidationScriptsPartial.cshtml` | Keep unchanged | Maintain client-side validation |

#### 2. New Shared Partial Views (Create)

| File | Purpose | Components Included |
|------|---------|-------------------|
| `Views/Shared/_Header.cshtml` | Main navigation with branding | Navbar, user menu, mobile burger |
| `Views/Shared/_Footer.cshtml` | Page footer | Copyright, links, navigation |
| `Views/Shared/_AlertMessages.cshtml` | TempData notifications | Success, error, warning, info messages |
| `Views/Shared/_Breadcrumbs.cshtml` | Navigation hierarchy | Breadcrumb trail with home link |
| `Views/Shared/_Pagination.cshtml` | Data table navigation | Page numbers, next/prev controls |
| `Views/Shared/_FormComponents.cshtml` | Reusable form elements | Input groups, validation states |
| `Views/Shared/_DataTables.cshtml` | Standard table layout | Sortable headers, responsive design |
| `Views/Shared/_DashboardCards.cshtml` | Statistics display | Metric cards with icons |
| `Views/Shared/_ModalConfirm.cshtml` | Confirmation dialogs | Delete confirmations, warnings |

#### 3. Static Asset Files

| File | Action | Purpose |
|------|--------|---------|
| `wwwroot/css/site.bulma.css` | Create | Custom Bulma overrides and app-specific styles |
| `wwwroot/js/ui.js` | Create | Bulma interactions, navbar toggle, modals |
| `wwwroot/css/site.css` | Remove | Replace with Bulma-based version |
| `wwwroot/js/site.js` | Remove | Replace with minimal UI-specific script |

#### 4. Authentication Views (Complete Rewrite)

| File | Purpose | Key Features |
|------|---------|-------------|
| `Views/Account/Login.cshtml` | User authentication | Login form with validation, remember me |
| `Views/Account/Register.cshtml` | User registration | Registration form with role selection |
| `Views/Account/ChangePassword.cshtml` | Password management | Change password form with validation |
| `Views/Account/Profile.cshtml` | User profile | Profile display and edit functionality |
| `Views/Account/AccessDenied.cshtml` | Error handling | Access denied message and navigation |

#### 5. Dashboard Views (Complete Rewrite)

| File | Purpose | Components |
|------|---------|------------|
| `Views/Dashboard/Index.cshtml` | Main dashboard | Statistics cards, charts, recent/upcoming meetings |

#### 6. Master Data Views (Complete Rewrite)

| File | Purpose | Features |
|------|---------|----------|
| `Views/MeetingType/Index.cshtml` | Meeting type listing | Search, create, edit, delete, export |
| `Views/MeetingType/AddEdit.cshtml` | Meeting type form | Validation, modal support |
| `Views/MeetingType/Details.cshtml` | Meeting type details | Read-only view with navigation |
| `Views/Department/Index.cshtml` | Department listing | Search, create, edit, delete, export |
| `Views/Department/AddEdit.cshtml` | Department form | Validation, modal support |
| `Views/Department/Details.cshtml` | Department details | Read-only view with staff members |
| `Views/Department/StaffMembers.cshtml` | Department staff | Staff listing by department |
| `Views/MeetingVenue/Index.cshtml` | Venue listing | Search, create, edit, delete, availability check |
| `Views/MeetingVenue/AddEdit.cshtml` | Venue form | Validation, capacity, facilities |
| `Views/MeetingVenue/Details.cshtml` | Venue details | Read-only view with meeting schedule |
| `Views/Staff/Index.cshtml` | Staff listing | Search, create, edit, delete, export |
| `Views/Staff/AddEdit.cshtml` | Staff form | Validation, department selection, email uniqueness |
| `Views/Staff/Details.cshtml` | Staff details | Read-only view with meeting history |

#### 7. Meeting Management Views (Complete Rewrite)

| File | Purpose | Features |
|------|---------|----------|
| `Views/Meeting/Index.cshtml` | Meeting listing | Search, filters, create, edit, cancel, export |
| `Views/Meeting/Create.cshtml` | Meeting creation | Scheduling form, conflict detection, file upload |
| `Views/Meeting/Edit.cshtml` | Meeting editing | Update form, conflict detection, file management |
| `Views/Meeting/Details.cshtml` | Meeting details | Full details, participants, documents, actions |
| `Views/Meeting/Cancel.cshtml` | Meeting cancellation | Cancellation form with reason |
| `Views/Meeting/Calendar.cshtml` | Calendar view | Monthly/weekly meeting schedule |
| `Views/Meeting/ManageAttendance.cshtml` | Attendance management | Mark attendance, add/remove participants |

#### 8. Meeting Member Views (Complete Rewrite)

| File | Purpose | Features |
|------|---------|----------|
| `Views/MeetingMember/Index.cshtml` | Attendance listing | Search by meeting/staff, export |
| `Views/MeetingMember/AttendanceDetails.cshtml` | Detailed attendance | Meeting attendance breakdown |
| `Views/MeetingMember/AttendanceReport.cshtml` | Reports | Staff participation statistics |
| `Views/MeetingMember/StaffAttendance.cshtml` | Staff history | Individual staff meeting record |

#### 9. Static Views

| File | Purpose | Action |
|------|---------|--------|
| `Views/Home/Index.cshtml` | Landing page | Rewrite with Bulma hero section |
| `Views/Home/Privacy.cshtml` | Privacy policy | Rewrite with Bulma styling |
| `Views/Shared/Error.cshtml` | Error display | Rewrite with Bulma error styling |

### File Structure Changes

#### New Directory Structure
```
Views/
├── Shared/
│   ├── _Layout.cshtml (rewritten)
│   ├── _Header.cshtml (new)
│   ├── _Footer.cshtml (new)
│   ├── _AlertMessages.cshtml (new)
│   ├── _Breadcrumbs.cshtml (new)
│   ├── _Pagination.cshtml (new)
│   ├── _FormComponents.cshtml (new)
│   ├── _DataTables.cshtml (new)
│   ├── _DashboardCards.cshtml (new)
│   ├── _ModalConfirm.cshtml (new)
│   └── _ValidationScriptsPartial.cshtml (unchanged)
├── Account/ (all views rewritten)
├── Dashboard/ (rewritten)
├── MeetingType/ (all views rewritten)
├── Department/ (all views rewritten)
├── MeetingVenue/ (all views rewritten)
├── Staff/ (all views rewritten)
├── Meeting/ (all views rewritten)
├── MeetingMember/ (all views rewritten)
├── Home/ (rewritten)
├── _ViewImports.cshtml (modified)
└── _ViewStart.cshtml (unchanged)

wwwroot/
├── css/
│   ├── site.bulma.css (new)
│   └── site.css (remove)
├── js/
│   ├── ui.js (new)
│   └── site.js (remove)
└── (existing asset directories unchanged)
```

### Dependencies and External Resources

#### CDN Resources to Add
```html
<!-- Bulma CSS -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bulma@0.9.4/css/bulma.min.css">

<!-- Font Awesome Icons -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">

<!-- Chart.js (for dashboard) -->
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
```

#### CDN Resources to Remove
```html
<!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">

<!-- Bootstrap Icons -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css">

<!-- Bootstrap JS -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

### Implementation Priority

#### Phase 1: Foundation (Priority 1)
1. `Views/_ViewImports.cshtml` - Update imports and tag helpers
2. `Views/Shared/_Layout.cshtml` - New master layout
3. `Views/Shared/_Header.cshtml` - Navigation component
4. `Views/Shared/_Footer.cshtml` - Footer component
5. `Views/Shared/_AlertMessages.cshtml` - Notification system
6. `wwwroot/css/site.bulma.css` - Base styling
7. `wwwroot/js/ui.js` - Core JavaScript interactions

#### Phase 2: Authentication (Priority 2)
1. `Views/Account/Login.cshtml` - Login page
2. `Views/Account/Register.cshtml` - Registration page
3. `Views/Account/AccessDenied.cshtml` - Error handling

#### Phase 3: Core Templates (Priority 3)
1. `Views/Dashboard/Index.cshtml` - Dashboard template
2. `Views/MeetingType/Index.cshtml` - List view template
3. `Views/MeetingType/AddEdit.cshtml` - Form template
4. `Views/Shared/_DataTables.cshtml` - Table component
5. `Views/Shared/_FormComponents.cshtml` - Form component
6. `Views/Shared/_Pagination.cshtml` - Pagination component

#### Phase 4: Remaining Views (Priority 4)
All remaining views in order of user flow:
1. Department, MeetingVenue, Staff views
2. Meeting management views
3. Meeting member/attendance views
4. Home and static pages

### Risk Assessment

#### High Risk Items
- **Layout compatibility**: Ensure all server-side TempData and ViewData work correctly
- **Form validation**: Client-side validation must integrate with Bulma styling
- **JavaScript conflicts**: Remove all Bootstrap dependencies
- **Responsive behavior**: Test across all breakpoints

#### Medium Risk Items
- **Icon mapping**: Bootstrap icons to Font Awesome conversion
- **Component behavior**: Ensure dropdowns, modals, and other interactive elements work
- **Chart integration**: Dashboard charts must render correctly

#### Low Risk Items
- **Styling updates**: Visual changes only, no functional impact
- **Content presentation**: Text and layout modifications
- **Navigation structure**: Menu organization and links

### Rollback Strategy
1. **Git branching**: Create feature branch for UI reimplementation
2. **Backup current views**: Maintain original files in separate branch
3. **Incremental deployment**: Deploy phases separately to test functionality
4. **Database independence**: No database changes required for UI update

### Success Criteria
1. All pages render without errors
2. All forms validate and submit correctly
3. All interactive elements function as expected
4. Responsive design works across all breakpoints
5. Accessibility standards are met (WCAG 2.1 AA)
6. Performance is maintained or improved
7. All existing functionality is preserved

---

*This file plan provides a comprehensive roadmap for completely reimagining the UI while maintaining full functional compatibility.*