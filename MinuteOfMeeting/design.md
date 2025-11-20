# UI Reimplementation Design & Decision Log
## Minutes of Meeting Management System - Bulma CSS Framework

### Project Overview
Complete UI reimplementation of an ASP.NET Core MVC meeting management system using Bulma CSS framework, replacing existing Bootstrap-based implementation while preserving all server-side functionality.

### Functional Requirements Summary

#### Core Features Identified
1. **Authentication System**
   - Login/Logout with session management
   - User registration
   - Password change functionality
   - Role-based access (Admin/Organizer/Staff)

2. **Master Data Management**
   - Meeting Types (CRUD)
   - Departments (CRUD)
   - Meeting Venues (CRUD)
   - Staff Members (CRUD)

3. **Meeting Management**
   - Meeting scheduling with conflict detection
   - File upload for meeting documents
   - Meeting cancellation with reason
   - Calendar view
   - Search and filtering

4. **Attendance Tracking**
   - Add participants to meetings
   - Mark attendance (present/absent)
   - Attendance reports and summaries

5. **Dashboard & Analytics**
   - Statistics cards (total, upcoming, completed, cancelled)
   - Charts (meetings by type, by department, monthly trends)
   - Recent and upcoming meetings lists

6. **Reporting & Export**
   - Excel export functionality
   - Filtered data export

### Design Decisions

#### 1. CSS Framework Choice: Bulma
**Decision**: Use Bulma CSS framework as the exclusive styling foundation.

**Rationale**:
- Modern, flexbox-based responsive design
- Comprehensive component library matching our needs
- Minimal learning curve for developers
- Excellent documentation and browser support
- No JavaScript dependencies (framework-agnostic)
- Clean, semantic class naming convention

#### 2. Accessibility Strategy (WCAG 2.1 AA Compliance)
**Implementation Approach**:
- Semantic HTML5 elements throughout
- Proper heading hierarchy (h1-h6)
- ARIA labels and roles where appropriate
- Keyboard navigation support
- Focus indicators enhanced with Bulma's `is-focused` states
- Screen reader-friendly form labels
- Color contrast meeting WCAG AA standards
- Skip links for navigation

#### 3. Responsive Design Strategy
**Breakpoint Mapping**:
- **Mobile**: ≤ 768px (Bulma's `touch` breakpoint)
- **Tablet**: 769px - 1023px (Bulma's `tablet` breakpoint)
- **Desktop**: ≥ 1024px (Bulma's `desktop` breakpoint)

**Layout Adaptations**:
- Mobile: Single column, stacked navigation, full-width cards
- Tablet: Two-column layouts where appropriate, horizontal navigation
- Desktop: Multi-column dashboards, side-by-side forms

#### 4. Component Mapping Strategy

| Bootstrap Component | Bulma Equivalent | Usage Context |
|-------------------|------------------|---------------|
| Navbar | `navbar` | Main navigation with brand, menu items, user dropdown |
| Cards | `card` | Dashboard widgets, meeting cards, staff profiles |
| Forms | `field` + `control` + `input` | All form implementations |
| Buttons | `button` | CRUD actions, navigation, form submissions |
| Tables | `table` | Data listings, reports |
| Modal | `modal` | Confirmations, detailed views |
| Alerts | `notification` | Success/error messages |
| Pagination | `pagination` | Data table navigation |
| Breadcrumb | `breadcrumb` | Navigation hierarchy |
| Dropdown | `dropdown` | Action menus, filters |

#### 5. Color Scheme & Branding
**Primary Colors**:
- Primary: `is-primary` (Blue) - Main actions, navigation
- Success: `is-success` (Green) - Success states, confirmations
- Warning: `is-warning` (Yellow) - Warnings, pending items
- Danger: `is-danger` (Red) - Delete actions, errors, cancelled meetings
- Info: `is-info` (Cyan) - Information, help text
- Light: `is-light` (Light gray) - Secondary backgrounds
- Dark: `is-dark` (Dark gray) - Text, contrast elements

#### 6. Typography Hierarchy
**Headings**:
- `title` class for main page titles (h1 equivalent)
- `subtitle` for section headings (h2-h3 equivalent)
- `heading` for sub-sections (h4-h6 equivalent)

**Body Text**:
- Default size for paragraphs
- `is-size-5/6/7` for variations
- `has-text-weight-light/normal/medium/semibold/bold` for weights

#### 7. Layout Architecture
**Main Layout Structure**:
```
- Hero (header) with navigation
- Section (main content area)
  - Container (content wrapper)
    - Level (horizontal layouts)
    - Columns (grid system)
      - Column (content areas)
- Footer (optional)
```

**Partial View Strategy**:
- `_Layout.cshtml` - Master template
- `_Header.cshtml` - Navigation and branding
- `_Footer.cshtml` - Footer content
- `_AlertMessages.cshtml` - Temp data notifications
- `_Breadcrumbs.cshtml` - Navigation breadcrumb
- `_Pagination.cshtml` - Data table pagination
- `_FormComponents.cshtml` - Reusable form elements

#### 8. Form Design Patterns
**Standard Form Layout**:
```html
<div class="field">
  <label class="label">Field Name</label>
  <div class="control has-icons-left/right">
    <input class="input" type="text" placeholder="Enter value">
    <span class="icon is-small is-left/right">
      <i class="fas fa-icon"></i>
    </span>
  </div>
  <p class="help is-success/danger/info">Validation message</p>
</div>
```

#### 9. Data Presentation Patterns
**Table Design**:
- `is-fullwidth` for responsive tables
- `is-hoverable` for interactive rows
- `is-striped` for readability
- `is-bordered` for clear boundaries

**Card Design**:
- `card` for content grouping
- `card-header` for titles
- `card-content` for main content
- `card-footer` for actions

#### 10. Interactive Elements Strategy
**JavaScript Requirements**:
- Navbar burger menu toggle (mobile)
- Modal show/hide functionality
- Dropdown interactions
- Dynamic form validation styling
- Chart integration (Chart.js)
- File upload progress indication

### Component Library Specifications

#### Navigation Components
- **Main Navbar**: Fixed top, responsive, with user menu
- **Breadcrumbs**: Hierarchical navigation
- **Sidebar**: Optional for dashboard (future enhancement)

#### Form Components
- **Text Inputs**: With validation states
- **Dropdowns**: For foreign key selection
- **Date Pickers**: Meeting scheduling
- **File Upload**: Document attachments
- **Checkboxes/Radio**: Attendance, options

#### Data Display Components
- **Data Tables**: Sortable, filterable listings
- **Cards**: Meeting summaries, statistics
- **Charts**: Dashboard analytics
- **Modals**: Confirmations, detailed views

#### Feedback Components
- **Notifications**: Success/error/warning messages
- **Progress Bars**: Loading states
- **Tooltips**: Help information
- **Badges**: Status indicators

### Performance Considerations
- **CSS**: Single compiled Bulma CSS file with minimal custom overrides
- **JavaScript**: Minimal, vanilla JS for Bulma interactions
- **Images**: Optimized icons from Font Awesome
- **Fonts**: System fonts to reduce loading time
- **Caching**: Proper cache headers for static assets

### Browser Support
- Modern browsers (Chrome, Firefox, Safari, Edge)
- IE11+ (with appropriate polyfills if needed)
- Mobile browsers (iOS Safari, Chrome Mobile)

### Future Enhancement Considerations
- Dark mode support using Bulma's color system
- Component variations for different user roles
- Advanced filtering interfaces
- Real-time updates (SignalR integration)
- Progressive Web App features

---

*This design document serves as the foundation for implementing a modern, accessible, and maintainable UI using Bulma CSS framework while preserving all existing functionality.*