import { renderAndGet } from "@test-utils/render.utils";
import { AuthProvider, Login } from "@core/auth";
import { MockComponent } from "@test-utils/mock-component";
import { makeElementByTypeFilter } from "@test-utils/find.utils";

describe("AuthProvider", () => {
  describe("Rendering", () => {
    it("Show child content", async () => {
      const renderResult = await renderAndGet(
        <AuthProvider>
          <MockComponent />
        </AuthProvider>
      );
      expect(
        renderResult.root.find(makeElementByTypeFilter(MockComponent))
      ).toBeTruthy();
    });
  });
});
