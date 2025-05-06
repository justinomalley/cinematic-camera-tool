# Unity Cinematic Camera Tool

![Camera Tool Playback Preview](demo.gif)

An in-development camera animation system for Unity that allows users to create, preview, and edit keyframe-based camera sequences.  
Supports Cut, Lerp, and Spline transitions, timeline editing, and WebGL previewing.

**Live Demo:** [https://jomal.space/camera-tool.html](https://jomal.space/camera-tool.html)

---

## Features

- Keyframe-based camera animation system
- Transition types: Cut, Lerp, and Spline
- Shot preview playback with UI controls
- Editor ↔ Preview view toggle for workflow testing
- Keyframe deletion and timeline editing
- Click-and-drag camera panning (editor-style viewport)
- Mousewheel zoom along the camera’s forward plane
- Hoverable, draggable preview camera with live screen-space render
- UI controls for adjusting camera position and Euler rotation
- Responsive WebGL deployment for easy sharing and feedback (desktop only)

---

## Tech Stack

- Unity 6 LTS
- WebGL Build Target (desktop browsers)

---

## Roadmap / Coming Soon

- Distance-based speed smoothing (arc-length parameterization for spline motion)
- Additional transition options and easing curves
- Keyframe reordering
- Export/import camera shot sequences as JSON
- Convert to Unity Editor Tool for in-editor shot creation

---

## Live Demo

**Try it now:** [https://jomal.space/camera-tool.html](https://jomal.space/camera-tool.html)

Compatible with modern desktop browsers.

---

## License

MIT License.  
Feel free to fork, modify, and use for personal or commercial projects. Attribution appreciated but not required.
