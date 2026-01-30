# GGJ Theme
"Mask"
- Idea is to take the less obvious definition of a mask in the sense of "masking" / a "mask" ala photoshop image mask - showing and hiding parts of an image
# Main idea summary
- Doodle graphics
- Low angle isometric view
- Puzzle platformer with a focus on puzzles
	- Puzzles based on painting/editing masks
	- Basic movement and platforming + interactable objects/scenes that open a "mask editor"
		1. When player interacts with an object, some sort of popup of the object's UV is show with a mask. The player gets to edit that mask
			- limited amount of "paint" to edit the mask
		2. When player is done editing, the mask is applied to the object, hiding/showing parts of the object
			- The mask essentially masks the albedo texture of the object to make parts of it transparent
		3. Transparent parts of the object have their collision turned off, so the player will fall through those parts of the object
		4. Player then platforms through the edited parts of the level
