using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Shapes;
using OrchardCore.DisplayManagement.Zones;
using OrchardCore.Modules;

namespace OrchardCore.Localization
{
    [Feature("OrchardCore.Localization.AdminCulturePicker")]
    public class AdminCulturePickerShapes : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Layout")
                .OnDisplaying(async context =>
                {
                    if (context.Shape is IZoneHolding layout)
                    {
                        var httpContextAccessor = context.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                        if (!AdminAttribute.IsApplied(httpContextAccessor.HttpContext))
                        {
                            return;
                        }

                        var navBarTop = layout.Zones["NavbarTop"];
                        if (navBarTop is Shape navBarTopShape)
                        {
                            var shapeFactory = context.ServiceProvider.GetRequiredService<IShapeFactory>();
                            var culturePickerShape = await shapeFactory.CreateAsync("AdminCulturePicker");
                            await navBarTopShape.AddAsync(culturePickerShape);
                        }
                    }
                });
        }
    }
}
